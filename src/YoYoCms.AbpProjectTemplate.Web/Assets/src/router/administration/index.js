/**
 * Created by huanghx on 2017/6/25.
 */
// const namePre = 'administration'
export default {
    path: '/administration',
    name: 'administration',
    component: resolve => {
        require.ensure([], () => {
            resolve(require('../../views/administration/Index.vue'))
        })
    },
    children: [{
        path: 'organizationunits',
        name: 'organizationUnits',
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/organizationUnits.vue'))
            })
        }
    }, {
        path: 'roles',
        name: `roles`,
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Roles.vue'))
            })
        }
    }]
}
