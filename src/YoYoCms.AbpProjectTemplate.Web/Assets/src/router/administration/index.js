/**
 * Created by huanghx on 2017/6/25.
 */
const namePre = 'administration'
export default {
    path: 'Administration',
    name: 'administration',
    component: resolve => {
        require.ensure([], () => {
            resolve(require('../../views/administration/Index.vue'))
        })
    },
    children: [{
        path: 'organizationunits',
        name: namePre + 'organizationUnits',
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/organizationUnits.vue'))
            })
        }
    }, {
        path: 'roles',
        name: namePre + 'roles',
        component: resolve => {
            require.ensure([], () => {
                resolve(require('../../views/administration/Roles.vue'))
            })
        }
    }]
}
