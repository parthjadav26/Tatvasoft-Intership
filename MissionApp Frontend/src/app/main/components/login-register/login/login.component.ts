import { NgIf } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Subscription } from 'rxjs';
import { APP_CONFIG } from 'src/app/main/configs/environment.config';
import { AuthService } from 'src/app/main/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit, OnDestroy {
  private unsubscribe: Subscription[] = [];
  loginForm: FormGroup;
  formValid: boolean = false;

  constructor(
    private _fb: FormBuilder,
    private _service: AuthService,
    private _router: Router,
    private _toast: NgToastService
  ) {}

  ngOnInit(): void {
    this.loginForm = this._fb.group({
      emailAddress: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required]],
    });
  }

  get emailAddress() {
    return this.loginForm.get('emailAddress') as FormControl;
  }

  get password() {
    return this.loginForm.get('password') as FormControl;
  }

  onSubmit() {
    this.formValid = true;

    if (this.loginForm.valid) {
      const { emailAddress, password } = this.loginForm.value;

      const loginSub = this._service.loginUser([emailAddress, password]).subscribe({
        next: (res: any) => {
          console.log('Login Response:', res);

          if (res.result === 1 && res.message === 'Login Successfully') {
            this._service.setToken(res.data);
            const tokenPayload = this._service.decodedToken();
            this._service.setCurrentUser(tokenPayload);

            this._toast.success({
              detail: 'SUCCESS',
              summary: res.message,
              duration: APP_CONFIG.toastDuration || 3000,
            });

            const isAdmin = tokenPayload.userType === 'admin';
            this._router.navigate([isAdmin ? 'admin/dashboard' : '/home']);
          } else {
            console.warn('Login failed:', res.message);

            this._toast.error({
              detail: 'ERROR',
              summary: res.message || 'Login failed',
              duration: APP_CONFIG.toastDuration || 3000,
            });
          }
        },
        error: (err) => {
          console.error('Login error:', err);

          this._toast.error({
            detail: 'ERROR',
            summary: 'Server error occurred.',
            duration: APP_CONFIG.toastDuration || 3000,
          });
        }
      });

      this.unsubscribe.push(loginSub);
      this.formValid = false;
    }
  }

  ngOnDestroy(): void {
    this.unsubscribe.forEach(sub => sub.unsubscribe());
  }
}
