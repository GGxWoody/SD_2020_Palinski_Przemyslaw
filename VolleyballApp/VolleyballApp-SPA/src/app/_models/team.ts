import { User } from './user';

export interface Team {
    id: number;
    teamName: string;
    owner?: User;
    users?: User[];
    dateCreated?: Date;
    description?: string;
}
