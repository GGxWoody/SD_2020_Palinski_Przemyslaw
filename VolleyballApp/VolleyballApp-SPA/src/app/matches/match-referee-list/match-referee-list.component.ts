import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NumberValueAccessor } from '@angular/forms';
import { Match } from 'src/app/_models/match';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { MatchService } from 'src/app/_services/match.service';

@Component({
  selector: 'app-match-referee-list',
  templateUrl: './match-referee-list.component.html',
  styleUrls: ['./match-referee-list.component.scss']
})
export class MatchRefereeListComponent implements OnInit {
  @Input() users: User[];
  @Input() pagination: Pagination;
  @Input() match: Match;
  @Output() matchChange: EventEmitter<Match> = new EventEmitter<Match>();
  @Input() userParams: any = {};

  constructor(private matchService: MatchService, private alertify: AlertifyService) { }

  ngOnInit() {
    console.log(this.users);
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadReferees();
  }

  loadReferees() {
    this.matchService.getMatchReferee(this.match.id, this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
    .subscribe((res: PaginatedResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

  sendMatchData() {
    this.matchChange.emit(this.match);
  }

}
