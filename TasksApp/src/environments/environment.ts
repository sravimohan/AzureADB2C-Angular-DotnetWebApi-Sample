export const environment = {
  // used in MsalFactory
  msal: {
    clientId: '4f44ffba-8042-49a7-9105-4eb81c9996da',
    tenantId: 'c512e30b-c327-43a9-9340-0d49119c380a',
    redirect: 'http://localhost:4200',
  },
  // used in MSALInterceptorConfigFactory
  graph: {
    uri: 'https://graph.microsoft.com/v1.0/me',
    scopes: ['openid', 'profile'],
  },
  // used in MSALInterceptorConfigFactory
  tasks_api: {
    uri: 'http://localhost:5250',
    scopes: ['api://tasks-api/tasks.read', 'api://tasks-api/tasks.write'],
  },
};
