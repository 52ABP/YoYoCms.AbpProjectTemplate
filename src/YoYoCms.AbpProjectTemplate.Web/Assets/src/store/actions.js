import * as types from './mutations'

const actions = {
    //  设置选中的导航
    setIndexMenuActive (store, {menu}) {
        if (abp.nav && menu) {
            menu.map((item) => {
                item.displayName = item.displayName || searchcDisplayName(item.name, abp.nav.menus.MainMenu)
            })
        }
        store.commit(types.INDEX_SETACTIVEMENU, {menu})
    },
    // 增加tab标签页
    addPageTab(store, {item}) {
        item.displayName = item.displayName || searchcDisplayName(item.name, abp.nav.menus.MainMenu)
        store.commit(types.INDEX_ADD_PAGETAB, {item})
    },
    // 删除tab标签页
    delPageTab(store, {name}) {
        store.commit(types.INDEX_DEL_PAGETAB, {name})
    },
    // 设置用户信息
    setAuthUser (store, {user}) {
        user.portrait = user.portrait || `/Profile/GetProfilePicture?v=${Date.now()}`
        store.commit(types.AUTH_SETUSER, {user})
    },
    // 设置未读数量
    setUnreadNotification (store, {count}) {
        store.commit(types.INDEX_SET_UNREADNOTIFICATION, {count})
    },
    // 设置右上角消息列表
    setNotifications (store, {data}) {
        store.commit(types.INDEX_SET_NOTIFICATIONS, {data})
    },
    // 向右上角消息列表增加一条数据
    pushNofications(store, {data}) {
        store.commit(types.INDEX_PUSH_NOTIFICATIONS, {data})
    },
    /**
     * 设置消息为已读
     * @param store
     * @param id 必填
     * @param data 选填 为列表中的item
     */
    setNotificationReaded(store, {id, data}) {
        store.commit(types.INDEX_SET_NOTIFICATIONSREADED, {id, data})
    }
}

// 从abp.nav中 获取菜单显示的名字
function searchcDisplayName(name, nav) {
    if (nav.name === name) return nav.displayName
    if (!nav.items || nav.items.length < 1) return null
    let ret
    for (let k in nav.items) {
        let item = nav.items[k]
        ret = searchcDisplayName(name, item)
        if (ret) break
    }
    return ret
}
export default actions
