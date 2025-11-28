import { Component, OnInit } from '@angular/core';
import { DestinationService } from '../Destinos/destination.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-destination-list',
  standalone: true,
  imports: [CommonModule], 
  templateUrl: './destination-list.html',
  styleUrls: ['./destination-list.css']
})
export class DestinationListComponent implements OnInit {

  destinations: any[] = [];

  constructor(private destinationService: DestinationService) {}

  ngOnInit() {
    this.destinationService.getDestinations().subscribe(data => {
      this.destinations = data;
    });
  }
}
