import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Match } from 'src/app/_models/match';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { MatchService } from 'src/app/_services/match.service';

@Component({
  selector: 'app-match-list',
  templateUrl: './match-list.component.html',
  styleUrls: ['./match-list.component.scss']
})
export class MatchListComponent implements OnInit {
  matches: Match[];
  pagination: Pagination;
  constructor(private matchService: MatchService, private alertify: AlertifyService, private root: ActivatedRoute) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.matches = data.matches.result;
      this.pagination = data.matches.pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadTeams();
  }

  loadTeams() {
    this.matchService.getMatches(this.pagination.currentPage, this.pagination.itemsPerPage)
    .subscribe((res: PaginatedResult<Match[]>) => {
      this.matches = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

}
