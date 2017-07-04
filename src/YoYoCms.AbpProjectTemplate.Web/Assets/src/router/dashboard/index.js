/**
 * Created by huanghx on 2017/7/4.
 */
export default { // 工作台
    path: '/dashboard',
    name: 'Dashboard.Tenant',
    component: resolve => {
        require.ensure([],
            () => {
                resolve(require('../../views/dashboard/Dashboard.vue'))
            })
    },
}