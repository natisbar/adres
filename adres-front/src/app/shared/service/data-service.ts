import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../../environment/environment';
import { HttpgeneralService } from '../../core/service/http-general.service';
import { Acquisition } from '../model/acquisition-model';
import { AcquisitionFilterDTO } from '../model/acquisition-filterDTO';


@Injectable({
  providedIn: 'root'
})
export class DataService {
  constructor(protected http: HttpgeneralService) {
  }

  public getHeader(contentTypeValue?: string){
    return new HttpHeaders({'Content-Type': 'application/json'});
  }

  getAllAcquisitions(): Observable<Acquisition[]> {
    const headers = this.getHeader();
    return this.http.doGet<Acquisition[]>(environment.endpoint, {headers}).pipe(
      catchError(this.handleError)
    );
  }

  getAllAcquisitionsFiltered(body: AcquisitionFilterDTO): Observable<Acquisition[]> {
    const headers = this.getHeader();
    return this.http.doPost<Acquisition[]>(environment.endpoint+"/filtro",body, {headers}).pipe(
      catchError(this.handleError)
    );
  }

  updateAcquisition(body: Acquisition, id: string){
    const headers = this.getHeader();
    return this.http.doPut(environment.endpoint+"/"+id, body, {headers}).pipe(
      catchError(this.handleError)
    );
  }

  deleteAcquisition(id: string){
    const headers = this.getHeader();
    return this.http.doDelete(environment.endpoint+"/"+id, {headers}).pipe(
      catchError(this.handleError)
    );
  }

  createAcquisition(body: Acquisition){
    const headers = this.getHeader();
    return this.http.doPost(environment.endpoint,body, {headers}).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error('Error del lado del cliente:', error.error.message);
    } else {
      console.error(`Código de error: ${error.status}`, error.error);
    }

    return throwError(() => error.error || 'Error desconocido');
  }
}