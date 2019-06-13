import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray, FormControl } from '@angular/forms';
import { AdminLinesService } from './admin-lines.service';
import { from } from 'rxjs';


@Component({
  selector: 'app-admin-lines',
  templateUrl: './admin-lines.component.html',
  styleUrls: ['./admin-lines.component.css']
})
export class AdminLinesComponent implements OnInit {

  stations : string[];
  selectedStations: string = "";
  addMssg : string;
  type : string = "";

  Form = this.fb.group({
    name: ['',Validators.required],
    stations : ['',Validators.required],
  });

  constructor(private formBuilder: FormBuilder, private service: AdminLinesService, private fb : FormBuilder) {

  }

  select(event : any)
  {
    this.type = event.target.value;
  }
  
 
  ngOnInit() {
    this.service.GetStations().subscribe((data)=> {
      this.stations = data;
    });
  }


  selectedStation(event: any) {
    //update the ui
    if(event.target.value != "")
    {
       this.selectedStations += event.target.value + ",";
    }
  }

AddLine()
{
  this.service.AddLine(this.Form.value.name,this.selectedStations,this.type).subscribe((data)=>{
      this.addMssg = data;
  });
  
}


}
