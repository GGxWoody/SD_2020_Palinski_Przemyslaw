import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Invite } from 'src/app/_models/invite';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FriendInviteService } from 'src/app/_services/friend-invite.service';

@Component({
  selector: 'app-invite-list',
  templateUrl: './invite-list.component.html',
  styleUrls: ['./invite-list.component.scss']
})
export class InviteListComponent implements OnInit {
  invites: Invite[];
  pagination: Pagination;
  constructor(private inviteService: FriendInviteService, private alertify: AlertifyService, private root: ActivatedRoute) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.invites = data.invites.result;
      this.pagination = data.invites.pagination;
      console.log(this.pagination);
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadInvites();
  }

  loadInvites() {
    this.inviteService.getInvites(this.pagination.currentPage, this.pagination.itemsPerPage)
    .subscribe((res: PaginatedResult<Invite[]>) => {
      this.invites = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

  refreshContent() {
    this.loadInvites();
  }

}
