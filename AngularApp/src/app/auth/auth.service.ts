import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, pipe, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from './user';
import { RegisterUser } from '../registerUser';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isLoggedIn = false;

  loginUrl: string = 'http://localhost:52295/oauth/token';

  constructor(private http: HttpClient) { }

  login(user: User): Observable<any> {
    return this.http.post<any>(this.loginUrl, `username=`+ user.username +`&password=`+ user.password + `&grant_type=password`, { 'headers': { 'Content-type': 'x-www-form-urlencoded' } }).pipe(
      map(res => {
        this.isLoggedIn = true;

        console.log(res.access_token);

        let jwt = res.access_token;

        let jwtData = jwt.split('.')[1]
        let decodedJwtJsonData = window.atob(jwtData)
        let decodedJwtData = JSON.parse(decodedJwtJsonData)

        let role = decodedJwtData.role

        console.log('jwtData: ' + jwtData)
        console.log('decodedJwtJsonData: ' + decodedJwtJsonData)
        console.log('decodedJwtData: ' + decodedJwtData)
        console.log('Role ' + role)

        localStorage.setItem('jwt', jwt)
        localStorage.setItem('role', role);
      }),

      catchError(this.handle2)
    );
  }

  logout(): void {
    this.isLoggedIn = false;
    localStorage.removeItem('jwt');
    localStorage.removeItem("role");
  }


  regiter(user : RegisterUser) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Account/Register', user).pipe(
      catchError(this.handle)
    );
  }

  sendImage(selectedFiles: File[], Email: string)
  {
    const sendImage = new FormData();
    for (let selectedFile of selectedFiles){
      sendImage.append(selectedFile.name, selectedFile)
    }    
    return this.http.post<any>("http://localhost:52295/api/Account/PostImage?Email="+Email ,sendImage).pipe(
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

  private handle2(error: any) {
    return of (error.message);
}
}
