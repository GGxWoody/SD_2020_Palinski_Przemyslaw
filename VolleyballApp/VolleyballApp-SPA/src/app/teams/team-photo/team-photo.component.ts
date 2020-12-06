import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { TeamService } from 'src/app/_services/team.service';
import { Team } from 'src/app/_models/team';

@Component({
  selector: 'app-team-photo',
  templateUrl: './team-photo.component.html',
  styleUrls: ['./team-photo.component.css']
})
export class TeamPhotoComponent implements OnInit {
  @Input() team: Team;
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: Photo;


  constructor(private authService: AuthService, private teamService: TeamService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
    url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos/' + this.team.id,
    authToken: 'Bearer ' + localStorage.getItem('token'),
    isHTML5: true,
    allowedFileType: ['image'],
    removeAfterUpload: true,
    autoUpload: false,
    maxFileSize: 10 * 1024 * 1024,
    queueLimit: 1,
    });
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
        };
        this.team.photo = photo;
      }
    };
  }


  deletePhoto(id: number) {
    this.alertify.confirm('Are you sure you want to delete this photo?', () => {
      this.teamService.deletePhoto(this.authService.decodedToken.nameid, id, this.team.id).subscribe(() => {
        this.team.photo = null;
        this.alertify.success('Photo has been deleted');
      }, error => {
        this.alertify.error('Failed to delete the photo');
      });
    });
  }
}
