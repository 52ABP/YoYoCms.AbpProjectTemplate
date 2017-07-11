/**
 * Created by huanghx on 2017/6/25.
 */
const namePre = 'common-'
export default {
    path: '/common',
    name: namePre,
    component: resolve => {
        require.ensure([], () => {
            resolve(require('../../views/common/Index.vue'))
        })
    },
    children: [{ // 消息列表
        path: 'notifications',
        name: namePre + 'notifications',
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/common/NotificationList.vue'))
            })
        },
        meta: {
            displayName: lang.L('Notifications')
        },
    }]
}
