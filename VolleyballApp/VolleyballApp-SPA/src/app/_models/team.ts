import { League } from './league';
import { Photo } from './photo';
import { User } from './user';

export interface Team {
    id: number;
    teamName: string;
    owner?: User;
    users?: User[];
    dateCreated?: Date;
    description?: string;
    photo?: Photo;
    photoUrl?: string;
    league?: League;
}
