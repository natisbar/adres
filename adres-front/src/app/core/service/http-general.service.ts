import { HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';


@Injectable({
    providedIn: 'root'
  })
export class HttpgeneralService {
  userName: string = 'user';
  password: string = 'password';

  constructor(public http: HttpClient) {}

  public doPost<T>(endpoint: string, body: any,  options?: { params?: HttpParams, headers?: HttpHeaders }): Observable<T>{

    return this.http.post<T>(endpoint, body, { ...options }) as Observable<T>;
  }

  public doPut<T>(endpoint: string, body: any,  options?: { params?: HttpParams, headers?: HttpHeaders }): Observable<T>{
    return this.http.put<T>(endpoint, body, { ...options }) as Observable<T>;
  }

  public doGet<T>(url: string, options?: { params?: HttpParams, headers?: HttpHeaders }){
    return this.http.get<T>(url, { ...options })
  }

  public doDelete<T>(endpoint: string, options?: { params?: HttpParams, headers?: HttpHeaders }){
    return this.http.delete<T>(endpoint, { ...options })
  }

  getHttpHeaders(): HttpHeaders {
    return new HttpHeaders().set('xhr-name', 'consultar registros');
  }
}