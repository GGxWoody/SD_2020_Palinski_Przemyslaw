import { User } from './user';

export interface Invite {
    id: number;
    inviteFrom: User;
    inviteTo: User;
    friendInvite: boolean;
    teamInvite: boolean;
    matchInvite: boolean;
}
