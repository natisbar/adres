export class Acquisition{
    id?: string;
    presupuesto!: number;
    unidad!: string;
    tipoBienServicio!: string;
    cantidad!: number;
    valorUnitario!: number;
    valorTotal?: number;
    fecha!: Date | string;
    proveedor!: string;
    documentacion!: string;
}
