import * as todoService from '../services/todo';

export default {
  namespace: 'todo',
  state: {
    list: [],
    total: null,
  },
  reducers: {
    save(state, { payload: { result } }) {
      return { ...state, list:result.items, total:result.totalCount };
    },
  },
  effects: {
    *fetch({ payload: { page } }, { call, put }) {
      const { data } = yield call(todoService.fetch, { page });
      yield put({ type: 'save', payload: data });
    },
  },
  subscriptions: {
    setup({ dispatch, history }) {
      return history.listen(({ pathname, query }) => {
        if (pathname === '/todo') {
          dispatch({ type: 'fetch', payload: query });
        }
      });
    },
  },
};