import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { profile } from './profile';

@Injectable({
  providedIn: 'root'
})
export class EditProfileService {

  constructor(private http: HttpClient) { }

  GetUserInfo() : Observable<any>
  {
   return this.http.get('http://localhost:52295/api/Account/UserInfo');
  }

  EditProfile(user: profile) : Observable<any>
  {
   return this.http.post('http://localhost:52295/api/Account/EditProfile',user).pipe(
    catchError(this.handle));
  }

   EditImage(selectedFiles: File[], Email: string)
  {
    const sendImage = new FormData();
    for (let selectedFile of selectedFiles){
      sendImage.append(selectedFile.name, selectedFile)
    }    
    return this.http.post<any>("http://localhost:52295/api/Account/EditImage?Email="+Email ,sendImage).pipe(
      catchError(this.handle)
      );
  }  

  ChangePassword(pass : any) : Observable<any>
  {
   return this.http.post('http://localhost:52295/api/Account/ChangePassword',pass).pipe(
    catchError(this.handle));
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
