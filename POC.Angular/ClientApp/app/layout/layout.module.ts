import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS} from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { AdminHeaderComponent } from './admin-header/admin-header.component';
import { SuperAdminLayoutComponent } from './superadmin-layout/superadmin-layout.component';
import { SuperAdminHeaderComponent } from './superadmin-header/superadmin-header.component';
import { SuperAdminFooterComponent } from './superadmin-footer/superadmin-footer.component';
import { InterceptorService } from '../shared/services/intercepter.service';
import { CustomHttpInterceptor } from '../shared/services/custom.httpinterceptor';
import { UserLayoutComponent } from './user-layout/user-layout.component';
import { UserHeaderComponent } from './user-header/user-header.component';

@NgModule({
    imports: [
        SharedModule
    ],
    providers: [
        InterceptorService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: CustomHttpInterceptor,
            multi:true
        }
    ],
    declarations: [
        AdminLayoutComponent,
        SuperAdminLayoutComponent,
        UserLayoutComponent,
        AdminHeaderComponent,        
        SuperAdminHeaderComponent,
        SuperAdminFooterComponent,
        UserHeaderComponent
    ],
    exports: [
        AdminLayoutComponent,
        SuperAdminLayoutComponent,
        UserLayoutComponent,
        AdminHeaderComponent,        
        SuperAdminHeaderComponent,
        SuperAdminFooterComponent,
        UserHeaderComponent
    ]
})
export class LayoutModule { }
