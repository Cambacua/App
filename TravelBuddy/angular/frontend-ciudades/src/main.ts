
import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { provideRouter } from '@angular/router';
import { appRoutes } from './app/app.routes';
import { App } from './app/app';

bootstrapApplication(App, {
  ...appConfig,
  providers: [
    ...appConfig.providers!,
    provideRouter(appRoutes)
  ]
});
/*bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));*/
