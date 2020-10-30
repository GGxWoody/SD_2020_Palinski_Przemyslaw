/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { FriendInviteService } from './friend-invite.service';

describe('Service: FriendInvite', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FriendInviteService]
    });
  });

  it('should ...', inject([FriendInviteService], (service: FriendInviteService) => {
    expect(service).toBeTruthy();
  }));
});
