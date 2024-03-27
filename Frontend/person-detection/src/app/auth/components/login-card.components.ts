import { Component, inject, signal } from '@angular/core';
import { lucideCheck, lucideChevronDown } from '@ng-icons/lucide';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import {
  HlmCardContentDirective,
  HlmCardDescriptionDirective,
  HlmCardDirective,
  HlmCardFooterDirective,
  HlmCardHeaderDirective,
  HlmCardTitleDirective,
} from '@spartan-ng/ui-card-helm';
import { BrnCommandImports } from '@spartan-ng/ui-command-brain';
import { HlmCommandImports } from '@spartan-ng/ui-command-helm';
import { HlmIconComponent, provideIcons } from '@spartan-ng/ui-icon-helm';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import { HlmPopoverContentDirective } from '@spartan-ng/ui-popover-helm';
import {
  BrnPopoverComponent,
  BrnPopoverContentDirective,
  BrnPopoverTriggerDirective,
} from '@spartan-ng/ui-popover-brain';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { ApiRoutesService } from '../../services/api-routes.service';
import { RegisterErrorInterface } from '../interfaces/register-error.interface';
import { AuthService } from '../services/auth.service';
import { UserInterface } from '../interfaces/user.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'login-card',
  standalone: true,
  imports: [
    BrnCommandImports,
    HlmCommandImports,
    HlmIconComponent,
    BrnPopoverComponent,
    BrnPopoverTriggerDirective,
    BrnPopoverContentDirective,
    HlmPopoverContentDirective,
    HlmCardDirective,
    HlmCardHeaderDirective,
    HlmCardTitleDirective,
    HlmCardDescriptionDirective,
    HlmCardContentDirective,
    HlmLabelDirective,
    HlmInputDirective,
    HlmCardFooterDirective,
    HlmButtonDirective,
    ReactiveFormsModule,
  ],
  providers: [provideIcons({ lucideCheck, lucideChevronDown })],
  template: `
    <section class="mx-auto my-8 max-w-md p-4 shadow" hlmCard>
      <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
        <div hlmCardHeader>
          <h3 hlmCardTitle>Login to your account</h3>
          <p hlmCardDescription>
            Enter your nickname, and password below to login to your account
          </p>
        </div>
        <div class="space-y-4" hlmCardContent>
          <label class="block" hlmLabel>
            Username
            <input
              class="mt-1.5 w-full"
              placeholder="Your Username"
              formControlName="username"
              hlmInput
            />
          </label>

          <label class="my-4 mb-1.5 block" hlmLabel>
            Password
            <input
              class="mt-1.5 w-full"
              type="password"
              placeholder="Your Password"
              formControlName="password"
              hlmInput
            />
          </label>
        </div>
        <div class="flex" hlmCardFooter>
          <button type="submit" class="w-full" hlmBtn>Login</button>
        </div>
      </form>
      <div class="flex items-center justify-center">
        <button
          hlmBtn
          variant="link"
          (click)="router.navigateByUrl('/register')"
          brnHoverCardTrigger
        >
          Don't have an account?
        </button>
      </div>
    </section>
  `,
})
export class LoginCardComponent {
  api = inject(ApiRoutesService);
  formBuilder = inject(FormBuilder);
  http = inject(HttpClient);
  auth = inject(AuthService);
  router = inject(Router);

  loginForm = this.formBuilder.nonNullable.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  ngOnInit() {
    if (this.auth.currentUser() !== null) {
      this.router.navigateByUrl('/');
    }
  }

  onSubmit() {
    this.http
      .post<UserInterface>(this.api.Login(), this.loginForm.value, {
        withCredentials: true,
      })
      .subscribe({
        next: (data) => {
          this.auth.currentUser.set(data);
          this.router.navigateByUrl('/');
        },
        error: (error) => {
          const errors = error.error.errors as RegisterErrorInterface;
          console.log(errors.Email);
        },
        complete: () => {},
      });
  }
}
