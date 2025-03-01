import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Acquisition } from '../../shared/model/acquisition-model';
import { DataService } from '../../shared/service/data-service';
import { ChangesMadeService } from '../../shared/service/changesMade-services';
import { ToastrService } from 'ngx-toastr';
import { moneyAmountValidator, regularCharacterValidator } from '../../shared/service/form-validator';

@Component({
  selector: 'app-add-acquisition',
  templateUrl: './add-acquisition.component.html',
  styleUrl: './add-acquisition.component.css'
})
export class AddAcquisitionComponent {
  public formAplicacion!: FormGroup;
  isModalOpen = false;
  changeMadeSubscription: any;
  reqFieldName: string = "*Este campo es requerido";
  numberField: string = "*Este campo debe ser numérico";
  errorMessage: string = '';

  constructor(private dataService: DataService,
    private changeMade: ChangesMadeService,
    private toastr: ToastrService
  ){

  }

  ngOnInit(){
    this.construirFormulario();

  }

  newAcquisitionObj(){
    const formValue = this.formAplicacion.value;
    const newAcquisition = {
      presupuesto: Number(formValue.presupuesto),
      unidad: formValue.unidad,
      tipoBienServicio: formValue.tipoBienServicio,
      cantidad: Number(formValue.cantidad),
      valorUnitario: Number(formValue.valorUnitario),
      fecha: formValue.fecha,
      proveedor: formValue.proveedor,
      documentacion: formValue.documentacion,
    }
    return newAcquisition;
  }

  addAcquisition(){
    if(this.formAplicacion.valid){
      const noteData = this.newAcquisitionObj();
      console.log(noteData)
      this.dataService.createAcquisition(noteData).subscribe({
        next: (response) =>{
          this.changeMade.notifyChange();
          this.formAplicacion.reset();
          this.toastr.success("Adquisición agregada.","Ëxito")
        },
        error: (error) =>{
          this.errorMessage = typeof error === 'string' ? error : JSON.stringify(error);
          this.toastr.error("No fue posible agregar la adquisición: "+this.errorMessage,"Error")
        }
      })
    }
    else{
      this.formAplicacion.markAllAsTouched();
      this.toastr.warning('Por favor, complete todos los campos requeridos.')
    }
  }

  openModal() {
    this.isModalOpen = true;
  }

  closeModal() {
    this.formAplicacion.reset();
    this.isModalOpen = false;
  }

  public construirFormulario(){
    this.formAplicacion = new FormGroup({
      presupuesto: new FormControl(null, [Validators.required, Validators.min(1)]),
      unidad: new FormControl(null, [Validators.required, Validators.minLength(3)]),
      tipoBienServicio: new FormControl(null, [Validators.required,Validators.minLength(3)]),
      cantidad: new FormControl(null, [Validators.required]),
      valorUnitario: new FormControl(null, [Validators.required, Validators.min(1)]),
      fecha: new FormControl(null, [Validators.required]),
      proveedor: new FormControl(null, [Validators.required, Validators.minLength(3)]),
      documentacion: new FormControl(null, [Validators.required, Validators.minLength(3)]),
    });
  }

  get presupuestoField() { return this.formAplicacion.get('presupuesto'); }
  get unidadField() { return this.formAplicacion.get('unidad'); }
  get tipoBienServicioField() { return this.formAplicacion.get('tipoBienServicio'); }
  get cantidadField() { return this.formAplicacion.get('cantidad'); }
  get valorUnitarioField() { return this.formAplicacion.get('valorUnitario'); }
  get fechaField() { return this.formAplicacion.get('fecha'); }
  get proveedorField() { return this.formAplicacion.get('proveedor'); }
  get documentacionField() { return this.formAplicacion.get('documentacion'); }
}
