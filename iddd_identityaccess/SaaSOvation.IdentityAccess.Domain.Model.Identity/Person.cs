﻿// Copyright 2012,2013 Vaughn Vernon
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SaaSOvation.IdentityAccess.Domain.Model.Identity
{
    using System;
    using SaaSOvation.Common.Domain.Model;

    public class Person
    {
        public Person(TenantId tenantId, FullName name, ContactInformation contactInformation)
        {
            this.ContactInformation = contactInformation;
            this.Name = name;
            this.TenantId = tenantId;
        }

        public ContactInformation ContactInformation { get; private set; }

        public EmailAddress EmailAddress
        {
            get
            {
                return this.ContactInformation.EmailAddress;
            }
        }

        public FullName Name { get; private set; }

        public TenantId TenantId { get; internal set; }

        public User User { get; internal set; }

        public void ChangeContactInformation(ContactInformation contactInformation)
        {
            this.ContactInformation = contactInformation;

            DomainEventPublisher
                .Instance
                .Publish(new PersonContactInformationChanged(
                        this.TenantId,
                        this.User.Username,
                        this.ContactInformation));
        }

        public void ChangeName(FullName name)
        {
            this.Name = name;

            DomainEventPublisher
                .Instance
                .Publish(new PersonNameChanged(
                        this.TenantId,
                        this.User.Username,
                        this.Name));
        }

        public override bool Equals(object anotherObject)
        {
            bool equalObjects = false;

            if (anotherObject != null && this.GetType() == anotherObject.GetType()) {
                Person typedObject = (Person) anotherObject;
                equalObjects =
                        this.TenantId.Equals(typedObject.TenantId) &&
                        this.User.Username.Equals(typedObject.User.Username);
            }

            return equalObjects;
        }

        public override int GetHashCode()
        {
            int hashCodeValue =
                + (90113 * 223)
                + this.TenantId.GetHashCode()
                + this.User.Username.GetHashCode();

            return hashCodeValue;
        }

        public override string ToString()
        {
            return "Person [tenantId=" + TenantId
                + ", name=" + Name
                + ", contactInformation=" + ContactInformation + "]";
        }
    }
}
