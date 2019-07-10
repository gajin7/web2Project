import { Component, OnInit } from '@angular/core';
import { ScheduleService } from './schedule.service';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {

  lineType : string = "";
  lines : string[];
  selectedLine : string = "";
  workDay: string[];
  saturday: string[];
  sunday: string[];

  LineForm = this.fb.group({
    LineName: [''],
   
  });

  constructor(public service: ScheduleService, private route: Router,private fb: FormBuilder) { }
  

  ngOnInit() {
  }

  selected (event: any) {
    //update the ui
    this.lineType = event.target.value;
   
    this.service.GetLines(this.lineType).subscribe((data) => { 
      this.lines = data; });
  }

 
  GetSchedules()
  {
    if(this.lineType == "")
    {
      window.alert("Please select type");
    }
    if(this.LineForm.value.LineName == "")
    {
      window.alert("Please select line");
    }
    this.service.GetSchedule("WorkDay",this.LineForm.value.LineName).subscribe((data) => { 
      this.workDay = data; });
    this.service.GetSchedule("Saturday",this.LineForm.value.LineName).subscribe((data) => { 
        this.saturday = data; });
    this.service.GetSchedule("Sunday",this.LineForm.value.LineName).subscribe((data) => { 
          this.sunday = data; });
  }

  Navigate()
  {
    if(localStorage.role == "AppUser")
    {
      this.route.navigate(['app-user-home']);
    }
    else
    {
      this.route.navigate(['home']);
    }
    
  }
}
