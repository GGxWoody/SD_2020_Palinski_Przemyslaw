import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Friend } from 'src/app/_models/friend';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-friend-list',
  templateUrl: './member-friend-list.component.html',
  styleUrls: ['./member-friend-list.component.scss']
})
export class MemberFriendListComponent implements OnInit {
  friends: User[];
  pagination: Pagination;
  user: User = JSON.parse(localStorage.getItem('user'));
  constructor(private userService: UserService, private alertify: AlertifyService, private root: ActivatedRoute) { }

  ngOnInit() {
    this.root.data.subscribe(data => {
      this.friends = data.friends.result;
      this.pagination = data.friends.pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadFriends();
  }


  loadFriends() {
    this.userService.getFriends(this.pagination.currentPage, this.pagination.itemsPerPage)
    .subscribe((res: PaginatedResult<User[]>) => {
      this.friends = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }
}
