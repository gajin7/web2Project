
import { StationModel } from './stationModel';

export class LineModel{
    Id: number;
    LineNumber: string;
    ColorLine: string;
    Stations: StationModel[] = [];
    Version: number;
    
    
    constructor( id: number,  linenumber:string,stations: StationModel[], col:string, ver? : number ){
        this.Id = id;
        this.LineNumber = linenumber;
        this.Stations = stations;
        this.ColorLine = col;
        this.Version = ver;
      
    }
}