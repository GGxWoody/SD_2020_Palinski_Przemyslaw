import { PhotoEditorComponent } from '../members/photo-editor/photo-editor.component';
import { Photo } from './photo';
import { Team } from './team';

export interface User {
    id: number;
    username: string;
    userType?: string;
    knownAs?: string;
    age?: number;
    gender?: string;
    created?: Date;
    photoUrl?: string;
    city?: string;
    country?: string;
    lastActive?: Date;
    photo?: Photo;
    intrests?: string;
    introduction?: string;
    lookingFor?: string;
    team?: Team;
    ownedTeam?: boolean;
    gamesWon?: number;
    gamesLost?: number;
    isFriend?: boolean;
}
