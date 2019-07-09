import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StationModel } from '../bus-maps/stationModel';

@Injectable({
  providedIn: 'root'
})
export class ForBusLocationService {

  url = "http://localhost:52295/";

  constructor(public http: HttpClient) { 

  }

  click(stations : StationModel[]): Observable<any> {
    let httpOptions = {
        headers:{
          "Content-type":"application/json"
        }
      } 

    return this.http.post(this.url+`api/BusStations/SendStationsToHub`,stations,httpOptions);
}

getAllLines() {
  return this.http.get("http://localhost:52295/api/BusStations/GetLinesForLocation");
}

}