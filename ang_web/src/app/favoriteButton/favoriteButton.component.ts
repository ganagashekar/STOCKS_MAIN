import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-favoriteButton',
  templateUrl: './favoriteButton.component.html',
  styleUrls: ['./favoriteButton.component.css']
})
export class FavoriteButtonComponent implements OnInit {


  @Input() selected: boolean = false;
  @Input() msnid: string ="";
  @Output() selectedChange = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  public toggleSelected() {
    debugger;
    this.selected = !this.selected;
    this.selectedChange.emit({msnid:this.msnid,selected:this.selected});
  }

}
