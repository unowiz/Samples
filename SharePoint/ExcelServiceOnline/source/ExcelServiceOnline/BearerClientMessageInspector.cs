﻿using Karamem0.Samples.SharePoint.ExcelServiceOnline.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Samples.SharePoint.ExcelServiceOnline {

    public class BearerClientMessageInspector : IClientMessageInspector {

        public void AfterReceiveReply(ref Message reply, object correlationState) {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel) {
            var property = new HttpRequestMessageProperty();
            property.Headers.Add("Authorization", "Bearer " + Settings.Default.AccessToken);
            request.Properties[HttpRequestMessageProperty.Name] = property;
            return null;
        }

    }

}
