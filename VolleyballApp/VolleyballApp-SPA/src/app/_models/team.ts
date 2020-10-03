import { User } from './user';

export interface Team {
    id: number;
    teamName: string;
    owner?: User;
    players?: User[];
    dateCreated?: Date;
    description?: string;
}
