import { Component, ElementRef, ViewChild } from '@angular/core';
import { DataService } from '../../shared/service/data-service';
import { Acquisition } from '../../shared/model/acquisition-model';
import { ChangesMadeService } from '../../shared/service/changesMade-services';
import { ToastrService } from 'ngx-toastr';
import { AcquisitionFilterDTO } from '../../shared/model/acquisition-filterDTO';
import { FormControl, FormGroup } from '@angular/forms';

const headers =  ['ID', 'PRESUPUESTO', 'UNIDAD', 'TIPO', 'CANTIDAD', 'VALOR U', 'VALOR T', 'FECHA', 'PROVEEDOR', 'DOCUMENTACION', 'ACCIONES'];

@Component({
  selector: 'app-dashboard-adquisitions',
  templateUrl: './dashboard-adquisitions.component.html',
  styleUrl: './dashboard-adquisitions.component.css'
})
export class DashboardAdquisitionsComponent {
  acquisitions: Acquisition[] = [];
  filteredAcquisitions: Acquisition[] = [];
  public tableHeaders = headers;
  public selectedAcquisition: Acquisition | undefined;
  isModalOpen = false;
  changeMadeSubscription: any;
  public formFilter!: FormGroup;
  showFilter = false;
  invalidBeforeDate: boolean = false;
  errorMessage: string = '';


  constructor(private dataService: DataService,
    private changeMade: ChangesMadeService,
    private toastr: ToastrService){

  }

  ngOnInit(){
    this.construirFormulario();
    this.filteredAcquisitions = this.acquisitions;
    this.listenForNewNote();
    this.getAllAcquisitionsFiltered({Presupuesto: null, Unidad: null, ValorTotal: null, FechaDesde: null, FechaHasta: null});
    this.changeMadeSubscription = this.changeMade.change$.subscribe(() => {
      this.getAllAcquisitionsFiltered({Presupuesto: null, Unidad: null, ValorTotal: null, FechaDesde: null, FechaHasta: null});
    });
  }

  toggleFilter() {
    this.showFilter = !this.showFilter;
  }

  listenForNewNote() {
    this.changeMade.change$.subscribe((change) => {
      if (change) {
        this.getAllAcquisitionsFiltered({Presupuesto: null, Unidad: null, ValorTotal: null, FechaDesde: null, FechaHasta: null});
        this.changeMade.resetNotification();
      }
    });
  }

  setAcquisitionToEdit(acquisition: Acquisition){
    this.selectedAcquisition = acquisition;
    this.isModalOpen = true;
  }
  
  closeModal() {
    this.isModalOpen = false;
  }

  deleteAcquisition(id: string){
    this.dataService.deleteAcquisition(id).subscribe({
      next: (response) =>{
        this.changeMade.notifyChange();
      },
      error: (error) =>{
        this.errorMessage = typeof error === 'string' ? error : JSON.stringify(error);
        this.toastr.error("No fue posible borrar la adquisición: "+this.errorMessage,"Error")
      }
    })
  }

  filterAcquisitions(){
    if (this.formFilter.valid){
      this.getAllAcquisitionsFiltered({
        Presupuesto: this.presupuestoField?.value, 
        Unidad: this.unidadField?.value, 
        ValorTotal: this.valorTotalField?.value, 
        FechaDesde: this.fechaDesdeField?.value, 
        FechaHasta: this.fechaHastaField?.value});
    }
  }

  validateDates(){
    if(this.fechaDesdeField && this.fechaHastaField){
      if(this.fechaDesdeField > this.fechaHastaField){
        this.invalidBeforeDate == true;
      }
    }
  }

  cleanFilter() {
    this.formFilter.reset();
    this.getAllAcquisitionsFiltered({Presupuesto: null, Unidad: null, ValorTotal: null, FechaDesde: null, FechaHasta: null});
  }
  
  getAllAcquisitionsFiltered(filter: AcquisitionFilterDTO){
    this.dataService.getAllAcquisitionsFiltered(filter).subscribe(
      data =>{
        this.acquisitions = data;
      },
      error =>{
        this.toastr.error("No fue posible obtener las adquisiciones.","Error")
      }
    )
  }

  public construirFormulario() {
    this.formFilter = new FormGroup({
      presupuesto: new FormControl(null, []),
      unidad: new FormControl(null, []),
      valorTotal: new FormControl(null, []),
      fechaDesde: new FormControl(null, []),
      fechaHasta: new FormControl(null, []),
    });
  }

  get presupuestoField() { return this.formFilter.get('presupuesto'); }
  get unidadField() { return this.formFilter.get('unidad'); }
  get valorTotalField() { return this.formFilter.get('valorTotal'); }
  get fechaDesdeField() { return this.formFilter.get('fechaDesde'); }
  get fechaHastaField() { return this.formFilter.get('fechaHasta'); }
}
