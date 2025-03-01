import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Acquisition } from '../../shared/model/acquisition-model';
import { FormControl, FormGroup } from '@angular/forms';
import { DataService } from '../../shared/service/data-service';
import { ChangesMadeService } from '../../shared/service/changesMade-services';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-acquisition',
  templateUrl: './edit-acquisition.component.html',
  styleUrl: './edit-acquisition.component.css'
})
export class EditAcquisitionComponent {
  @Input()
  acquisition: Acquisition | undefined;
  public formAplicacion!: FormGroup;
  @Output() close = new EventEmitter<void>();
  changeMadeSubscription: any;
  errorMessage: string = '';


  constructor(private dataService: DataService,
    private changeMade: ChangesMadeService,
    private toastr: ToastrService
  ){

  }

  ngOnInit(){
    this.construirFormulario();
    console.log(this.cantidadField)
  }

  closeModal() {
    this.acquisition = undefined;
    this.close.emit();
  }

  saveChanges() {
    console.log('Datos guardados:', this.acquisition);
    this.closeModal();
  }

  newAcquisitionObj(){
    const formValue = this.formAplicacion.value;
    const newAcquisition = {
      id: this.acquisition?.id,
      presupuesto: Number(formValue.presupuesto),
      unidad: formValue.unidad,
      tipoBienServicio: formValue.tipoBienServicio,
      cantidad: Number(formValue.cantidad),
      valorUnitario: Number(formValue.valorUnitario),
      valorTotal: Number(formValue.valorTotal),
      fecha: formValue.fecha,
      proveedor: formValue.proveedor,
      documentacion: formValue.documentacion,
      // activo: Boolean(formValue.activo)
    }
    return newAcquisition;
  }

  editAcquisition(){
    if(this.formAplicacion.valid && this.acquisition!.id){
      const noteData = this.newAcquisitionObj();
      this.dataService.updateAcquisition(noteData, this.acquisition!.id).subscribe({
        next: (response) =>{
          this.changeMade.notifyChange();
          this.formAplicacion.reset();
          this.toastr.success("Adquisición editado.","Ëxito")
        },
        error: (error) =>{
          this.errorMessage = typeof error === 'string' ? error : JSON.stringify(error);
          console.log(error)
          this.toastr.error("No fue posible editar la adquisición: "+this.errorMessage,"Error")
        }
      })
    }
    else{
      this.formAplicacion.markAllAsTouched();
      this.toastr.warning('Por favor, complete todos los campos requeridos.')
    }
  }

  public construirFormulario() {
    if (this.acquisition) {
      const fechaISO = new Date(this.acquisition.fecha).toISOString().split('T')[0];

      this.formAplicacion = new FormGroup({
        presupuesto: new FormControl(this.acquisition.presupuesto, []),
        unidad: new FormControl(this.acquisition.unidad, []),
        tipoBienServicio: new FormControl(this.acquisition.tipoBienServicio, []),
        cantidad: new FormControl(this.acquisition.cantidad, []),
        valorUnitario: new FormControl(this.acquisition.valorUnitario, []),
        valorTotal: new FormControl(this.acquisition.valorTotal, []),
        fecha: new FormControl(fechaISO, []),
        proveedor: new FormControl(this.acquisition.proveedor, []),
        documentacion: new FormControl(this.acquisition.documentacion, []),
        // activo: new FormControl(this.acquisition.activo, []),
      });
    }
  }


  get presupuestoField() { return this.formAplicacion.get('presupuesto'); }
  get unidadField() { return this.formAplicacion.get('unidad'); }
  get tipoBienServicioField() { return this.formAplicacion.get('tipoBienServicio'); }
  get cantidadField() { return this.formAplicacion.get('cantidad'); }
  get valorUnitarioField() { return this.formAplicacion.get('valorUnitario'); }
  get valorTotalField() { return this.formAplicacion.get('valorTotal'); }
  get fechaField() { return this.formAplicacion.get('fecha'); }
  get proveedorField() { return this.formAplicacion.get('proveedor'); }
  get documentacionField() { return this.formAplicacion.get('documentacion'); }
  // get activeField() { return this.formAplicacion.get('active'); }
}
