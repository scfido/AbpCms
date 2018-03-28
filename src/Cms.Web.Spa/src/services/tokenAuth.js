import request from '../utils/request';

export async function authenticate(params) {
  return request('/api/TokenAuth/Authenticate', {
    method: 'POST',
    body: params,
  });
}
