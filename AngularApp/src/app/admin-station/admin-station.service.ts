import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Station } from './station';

@Injectable({
  providedIn: 'root'
})
export class AdminStationService {

  constructor(private http: HttpClient) { }

  AddStation(st : Station) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/AddStation',st).pipe(
      catchError(this.handle)
    );
  }

  DeleteStation(id: string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/DeleteStation',`id=`+id,{ 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  private handle(error: any) {
    return of (error.error.Message);
}
}
