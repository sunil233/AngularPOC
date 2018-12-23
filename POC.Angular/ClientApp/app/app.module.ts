import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; 
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';

import { LayoutModule } from './layout/layout.module';
import { SharedModule } from './shared/shared.module';
import { PagesModule } from './pages/pages.module';
import { AppConfig, ConfigModule } from './app.configuration';
import { AuthenticationService } from './services/auth.service';
import { ToastrModule } from 'ngx-toastr';


@NgModule({
    declarations: [
        AppComponent
     ],
  imports: [
      HttpClientModule,
      BrowserAnimationsModule,
      ToastrModule.forRoot(), // ToastrModule added
      BrowserModule,    
      LayoutModule,
      SharedModule.forRoot(),
      PagesModule     
  ],
    providers: [AppConfig,
        ConfigModule.init(), AuthenticationService],
  bootstrap: [AppComponent]
})
export class AppModule { }
