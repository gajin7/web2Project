import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdminPriceListService {

  constructor(private http: HttpClient) { }

  public ChangeTicket(type : string, price: string,version :string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/ChangePrice', `type=` + type + `&price=` + price + "&version="+ version,{ 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  public ChangeDiscount(type : string, discount: string,version :string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/ChangeDiscount', `type=` + type + `&discount=` + discount + "&version="+ version,{ 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  public GetPriceListVersions() : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/GetPriceListVersions', { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  public GetDiscountsVersions() : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/GetDiscountsVersions', { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }

  private handle(error: any) {
    return of (error.error.Message);
}
}
