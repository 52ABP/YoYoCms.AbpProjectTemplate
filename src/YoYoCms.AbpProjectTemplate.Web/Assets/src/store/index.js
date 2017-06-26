import Vue from 'vue'
import Vuex from 'vuex'
import createLogger from 'vuex/dist/logger'
import config from '../common/config'
import actions from './actions'
import auth from './modules/auth'
import index from './modules/index'
Vue.use(Vuex)

const store = new Vuex.Store({
    actions,
    modules: {
        auth,
        index
    },
    strict: config.isDebug,
    plugins: config.isDebug ? [createLogger()] : []
})

export default store
