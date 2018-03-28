import { connect } from 'dva';
import { Table, Pagination, Popconfirm } from 'antd';
import styles from './Todos.css';
import { PAGE_SIZE } from '../constants';

function Todos({ list: dataSource, total, page: current }) {
  function deleteHandler(id) {
    console.warn(`TODO: delete 
    ${id}`);
  }

  const columns = [
    {
      title: '标题',
      dataIndex: 'title',
      key: 'title',
      render: text => <a href="">{text}</a>,
    },
    {
      title: 'Id',
      dataIndex: 'id',
      key: 'id',
    },
    {
      title: '操作',
      key: 'operation',
      render: (text, { id }) => (
        <span className={styles.operation}>
          <a href="">Edit</a>
          <Popconfirm title="Confirm to delete?" onConfirm={deleteHandler.bind(null, id)}>
            <a href="">Delete</a>
          </Popconfirm>
        </span>
      ),
    },
  ];

  return (
    <div className={styles.normal}>
      <div>
        <Table
          columns={columns}
          dataSource={dataSource}
          rowKey={record => record.id}
          pagination={false}
        />
        <Pagination
          className="ant-table-pagination"
          total={total}
          current={current}
          pageSize={PAGE_SIZE}
        />
      </div>
    </div>
  );
}

function mapStateToProps(state) {
  const { list, total, page } = state.todo;
  return {
    list,
    total,
    page,
  };
}

export default connect(mapStateToProps)(Todos);