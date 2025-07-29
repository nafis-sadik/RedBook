import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NbThemeModule, NbLayoutModule, NbSidebarModule, NbMenuModule, NbWindowModule, NbToastrModule, NbGlobalPhysicalPosition, NbDialogModule, NbDatepickerModule } from '@nebular/theme';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NbPasswordAuthStrategy, NbAuthModule, NbAuthJWTToken } from '@nebular/auth';
import { AuthGuard } from './shared/auth/auth-guard.service';
import { AuthInterceptorService } from './shared/auth/api-Interceptor.service';
import { environment } from 'src/environments/environment';
import { OAuthModule } from 'angular-oauth2-oidc';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NbEvaIconsModule,
    NbThemeModule.forRoot({ name: 'default' }),
    NbToastrModule.forRoot({
      preventDuplicates: false,
      destroyByClick: true,
      position: NbGlobalPhysicalPosition.TOP_RIGHT,
    }),
    NbDialogModule.forChild({
      autoFocus: true,
      closeOnEsc: true,
      hasBackdrop: true,
      hasScroll: true
    }),
    NbDatepickerModule.forRoot(),
    NbWindowModule.forChild(),
    NbSidebarModule.forRoot(),
    NbMenuModule.forRoot(),
    NbLayoutModule,
    NbEvaIconsModule,
    HttpClientModule,
    NbAuthModule.forRoot({
      strategies: [
        NbPasswordAuthStrategy.setup({
          name: 'email',
          token: {
            class: NbAuthJWTToken,
            key: 'response', // key where the token is located in local storage
          },
          baseEndpoint: `${environment.baseUrlUMS}`,
          // baseEndpoint: `http://localhost:5062`,
          login: {
            endpoint: '/api/Auth/LogIn',
            redirect: {
              success: '/dashboard',
            },
          },
        }),
      ],
      forms: {},
    }),
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: [environment.baseUrlUMS],
        sendAccessToken: true,
      },
    }),
  ],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
