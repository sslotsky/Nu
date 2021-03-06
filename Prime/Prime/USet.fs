﻿// Prime - A PRIMitivEs code library.
// Copyright (C) Bryan Edds, 2012-2016.

namespace Prime
open System.Collections
open System.Collections.Generic

[<AutoOpen>]
module USetModule =

    type [<NoEquality; NoComparison>] USet<'a when 'a : comparison> =
        private
            { RefSet : 'a TSet ref }
    
        interface IEnumerable<'a> with
            member this.GetEnumerator () =
                let (seq, tset) = TSet.toSeq !this.RefSet
                this.RefSet := tset
                seq.GetEnumerator ()
    
        interface IEnumerable with
            member this.GetEnumerator () =
                (this :> IEnumerable<'a>).GetEnumerator () :> IEnumerator

    [<RequireQualifiedAccess>]
    module USet =

        let makeEmpty<'a when 'a : comparison> bloatFactorOpt =
            { RefSet = ref ^ TSet.makeEmpty<'a> bloatFactorOpt }

        let add value set =
            { RefSet = ref ^ TSet.add value !set.RefSet }

        let remove value set =
            { RefSet = ref ^ TSet.remove value !set.RefSet }
    
        /// Add all the given values to the set.
        let addMany values set =
            { RefSet = ref ^ TSet.addMany values !set.RefSet }
    
        /// Remove all the given values from the set.
        let removeMany values set =
            { RefSet = ref ^ TSet.removeMany values !set.RefSet }

        let isEmpty set =
            let (result, tset) = TSet.isEmpty !set.RefSet
            set.RefSet := tset
            result

        let notEmpty set =
            not ^ isEmpty set

        let contains value set =
            let (result, tset) = TSet.contains value !set.RefSet
            set.RefSet := tset
            result

        let ofSeq items =
            { RefSet = ref ^ TSet.ofSeq items }

        let toSeq (set : _ USet) =
            set :> _ seq

        let fold folder state set =
            let (result, tset) = TSet.fold folder state !set.RefSet
            set.RefSet := tset
            result

        let map mapper set =
            let (result, tset) = TSet.map mapper !set.RefSet
            set.RefSet := tset
            { RefSet = ref result }

        let filter pred set =
            let (result, tset) = TSet.filter pred !set.RefSet
            set.RefSet := tset
            { RefSet = ref result }

type USet<'a when 'a : comparison> = USetModule.USet<'a>