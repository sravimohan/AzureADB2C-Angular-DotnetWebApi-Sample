export const environment = {
  msal: {
    clientId: '4f44ffba-8042-49a7-9105-4eb81c9996da',
    tenantId: 'c512e30b-c327-43a9-9340-0d49119c380a',
    redirect: 'http://localhost:4200',
  },
  graph: {
    uri: 'https://graph.microsoft.com/v1.0/me',
    scopes: ['openid', 'profile'],
  },
};
