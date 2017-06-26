import * as types from './mutations'

const actions = {
    // 设置首页网站路径
    setIndexWebpath (store, {main, sub}) {
        store.commit(types.INDEX_SETWEBPATH, {main, sub})
    },
    // 设置导航菜单
    setIndexNavMenu (store, {menu}) {
        store.commit(types.INDEX_SET_NAVMENU, {menu})
    },
    //  设置选中的导航
    setIndexMenuActive (store, url) {
        store.commit(types.INDEX_SET__MENU_ACTIVE, url)
    },
    // 清空列表的筛选条件
    clearFetchParam (store, need) {
        store.commit(types.INDEX_SET__CLEARFETCHPARAM, need)
    }
}

export default actions
