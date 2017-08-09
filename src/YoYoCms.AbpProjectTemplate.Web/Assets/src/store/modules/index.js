/**
 * Created by huanghuixin on 2017/3/29.
 */
import Vue from 'vue'
import {
    INDEX_SETACTIVEMENU,
    INDEX_SET_NOTIFICATIONS,
    INDEX_SET_UNREADNOTIFICATION,
    INDEX_PUSH_NOTIFICATIONS,
    INDEX_SET_NOTIFICATIONSREADED,
    INDEX_DEL_PAGETAB,
    INDEX_ADD_PAGETAB
} from '../mutations'

const Auth = {
    state: {
        webpathMain: '主页',
        webpathSub: '',
        navMenueActive: [], // 当前菜单的选中项
        navShow: [],
        unReadNotification: 0, // 未读数量
        notifications: [], // 消息列表
        pageTab: [{
            displayName: '工作台',
            url: '',
            name: 'Dashboard.Tenant',
            isActive: true,
            query: void 0,
            params: void 0
        }],
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
            if (!id) {
                state.notifications.forEach((item) => {
                    if (item.id == data.id) data = item
                })
            }
            if (!data) throw Object('没找到该消息!')
            if (data.state == 0) state.unReadNotification--
            data.state = 1
        },
        // 删除标签页
        [INDEX_DEL_PAGETAB](state, {name}) {
            for (let i = 0; i < state.pageTab.length; i++) {
                let item = state.pageTab[i]
                if (item.name === name) {
                    state.pageTab.splice(i, 1)
                    return
                }
            }
        },
        // 增加标签页
        [INDEX_ADD_PAGETAB](state, {item}) {
            let isExist = false
            // 判断当前的标签页是否已经存在过
            for (let i = 0; i < state.pageTab.length; i++) {
                let currItem = state.pageTab[i]
                currItem.isActive = false
                if (currItem.name === item.name) {
                    currItem.isActive = true
                    Vue.set(state.pageTab, i, currItem)
                    isExist = true
                }
            }

            if (isExist) return
            item.isActive = true
            state.pageTab.push(item)
        }
    },
}

export default Auth
