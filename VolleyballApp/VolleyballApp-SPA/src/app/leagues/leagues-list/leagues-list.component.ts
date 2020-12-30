import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { League } from 'src/app/_models/league';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { LeaguesService } from 'src/app/_services/leagues.service';

@Component({
  selector: 'app-leagues-list',
  templateUrl: './leagues-list.component.html',
  styleUrls: ['./leagues-list.component.scss']
})
export class LeaguesListComponent implements OnInit {
  creationMode = false;
  leagues: League[];
  pagination: Pagination;
  user: User = JSON.parse(localStorage.getItem('user'));
  constructor(private alertify: AlertifyService, private root: ActivatedRoute,
              private leaguesService: LeaguesService) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.leagues = data.leagues.result;
      this.pagination = data.leagues.pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadLeagues();
  }


  loadLeagues() {
    this.leaguesService.getLeagues(this.pagination.currentPage, this.pagination.itemsPerPage)
    .subscribe((res: PaginatedResult<League[]>) => {
      this.leagues = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

  creationToggle() {
    this.creationMode = true;
  }

  cancelCreationMode(registerMode: boolean) {
    this.creationMode = registerMode;
  }
}
