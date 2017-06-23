import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
    routes: [
        {
            path: '/login',
            name: 'login',
            component: resolve => {
                require.ensure([], () => {
                    resolve(require('../views/loginregist/Login.vue'))
                })
            }
        },
        {
            path: '/',
            name: 'index',
            component: resolve => {
                require.ensure([], () => {
                    resolve(require('../views/Index.vue'))
                })
            }
        }
    ],
    base: '/view/',
    mode: 'history',
})
