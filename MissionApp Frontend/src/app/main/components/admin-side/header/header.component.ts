import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import dateFormat from 'dateformat';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/main/services/auth.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { APP_CONFIG } from 'src/app/main/configs/environment.config';
import { CommonModule } from '@angular/common';
import { NgToastService } from 'ng-angular-popup';
import { ClientService } from 'src/app/main/services/client.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [BsDropdownModule, RouterModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit, OnDestroy {
  data: string = '';
  userDetail: string = 'User';
  loggedInUserDetail: any;
  private unsubscribe: Subscription[] = [];

  constructor(
    private _service: AuthService,
    private _clientService: ClientService,
    public _router: Router,
    private _toast: NgToastService
  ) {
    // Update clock every second
    setInterval(() => {
      const now = new Date();
      this.data = dateFormat(now, 'dddd mmmm dS, yyyy, h:MM:ss TT');
    }, 1000);
  }

  ngOnInit(): void {
    const user = this._service.decodedToken();
    if (user) {
      this.loggedInUserDetail = user;
      this.userDetail = user.fullName || 'User';
       this.loginUserDetailByUserId(user.id);
    }
  }

  getFullImageUrl(relativePath: string): string {
    if (!relativePath) return 'assets/Images/default-user.png';
    if (relativePath.startsWith('http')) return relativePath;
    return `${APP_CONFIG.imageBaseUrl}/${relativePath}?t=${Date.now()}`;
  }

  onImageError(event: Event) {
    (event.target as HTMLImageElement).src = 'assets/Images/default-user.png';
  }

  onLogoutClick(event: Event): void {
    event.preventDefault();
    event.stopPropagation();
    this.loggedOut();
  }

  loggedOut(): void {
    this._service.loggedOut();
    this._router.navigate(['']);
  }

  ngOnDestroy(): void {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }

  loginUserDetailByUserId(id: any): void {
    const sub = this._clientService.loginUserDetailById(id).subscribe(
      (data: any) => {
        if (data.result === 1) {
          this.loggedInUserDetail = data.data;
        } else {
          this._toast.error({
            detail: 'ERROR',
            summary: data.message,
            duration: APP_CONFIG.toastDuration,
          });
        }
      },
      (err) =>
        this._toast.error({
          detail: 'ERROR',
          summary: err.message,
          duration: APP_CONFIG.toastDuration,
        })
    );
    this.unsubscribe.push(sub);
  }
}
