import { User } from './user';

export interface Friend {
    id: number;
    firstUser: User;
    secondUser: User;
}
