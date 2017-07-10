/**
 * Created by huanghuixin on 2017/3/29.
 */
import {
    INDEX_SETACTIVEMENU,
    INDEX_SET_NOTIFICATIONS,
    INDEX_SET_UNREADNOTIFICATION,
    INDEX_PUSH_NOTIFICATIONS,
    INDEX_SET_NOTIFICATIONSREADED
} from '../mutations'

const Auth = {
    state: {
        webpathMain: '主页',
        webpathSub: '',
        navMenueActive: [], // 当前菜单的选中项
        navShow: [],
        unReadNotification: 0, // 未读数量
        notifications: [], // 消息列表
    },

    mutations: {
        // 设置选中的菜单
        [INDEX_SETACTIVEMENU] (state, {menu}) {
            state.navMenueActive = menu

            if (menu && menu.length > 1) {
                let navShow = menu.filter((item) => {
                    return !!item.displayName
                })
                state.navShow = navShow || []
            }
        },
        // 设置消息列表
        [INDEX_SET_NOTIFICATIONS] (state, {data}) {
            state.notifications = data
        },
        // 设置未读消息数量
        [INDEX_SET_UNREADNOTIFICATION] (state, {count}) {
            state.unReadNotification = count
        },
        // 消息列表增加一条数据
        [INDEX_PUSH_NOTIFICATIONS](state, {data}) {
            state.unReadNotification++
            state.notifications.unshift(data)
        },
        // 设置消息为已读
        [INDEX_SET_NOTIFICATIONSREADED](state, {id, data}) {
            if (!data) {
                state.notifications.forEach((item) => {
                    if (item.id == id) data = item
                })
            }
            if (!data) throw Object('没找到该消息!')
            if (data.state == 0) state.unReadNotification--
            data.state = 1
        }
    },
}

export default Auth
