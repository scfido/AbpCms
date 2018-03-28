import request from '../../../utils/request';
import { PAGE_SIZE } from '../constants';

export function fetch({ skip = 0 }) {
  return request(`/api/services/app/Todo/GetAll?SkipCount=${skip}&MaxResultCount=${PAGE_SIZE}`);
}